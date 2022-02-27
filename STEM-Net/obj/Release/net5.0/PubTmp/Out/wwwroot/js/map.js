var map, datasource, client, popup, searchInput, resultsPanel, searchInputLength, centerMapOnResults;

//The minimum number of characters needed in the search input before a search is performed.
var minSearchInputLength = 3;

//The number of ms between key strokes to wait before performing a search.
var keyStrokeDelay = 150;

function GetMap() {
    //Initialize a map instance.
    map = new atlas.Map('myMap', {
        center: [-121.885581, 37.337715],
        zoom: 15,
        view: 'Auto',

        //Add authentication details for connecting to Azure Maps.
        authOptions: {
            //Use Azure Active Directory authentication.
            authType: 'anonymous',
            clientId: "4e169728-0fec-4c3e-bb83-8f1cd15e966d", //Your Azure Active Directory client id for accessing your Azure Maps account.
            getToken: function (resolve, reject, map) {
                //URL to your authentication service that retrieves an Azure Active Directory Token.
                var tokenServiceUrl = "https://stemnetpythontestapp.azurewebsites.net/api/httptrigger2";

                fetch(tokenServiceUrl).then(r => r.text()).then(token => resolve(token));
            }

            //Alternatively, use an Azure Maps key. Get an Azure Maps key at https://azure.com/maps. NOTE: The primary key should be used as the key.
            //authType: 'subscriptionKey',
            //subscriptionKey: 'GghLY7LtRJz0jBKBw2RuArtyJ1VofzRCmkOJApynNS8'
        }
    });

    //Store a reference to the Search Info Panel.
    resultsPanel = document.getElementById("results-panel");

    //Add key up event to the search box. 
    searchInput = document.getElementById("search-input");
    searchInput.addEventListener("keyup", searchInputKeyup);

    //Create a popup which we can reuse for each result.
    popup = new atlas.Popup();

    //Wait until the map resources are ready.
    map.events.add('ready', function () {
        //Create a data source and add it to the map.
        datasource = new atlas.source.DataSource();
        map.sources.add(datasource);

        //Import the GeoJSON data into the data source. Capture the point data after it has loaded for quick lookups later.
        //datasource.importDataFromUrl("https://azuremapscodesamples.azurewebsites.net/Common/data/geojson/SamplePoiDataSet.json");
        datasource.importDataFromUrl("../res/small.json");

        //Create a layer to render the points.
        var pointLayer = new atlas.layer.BubbleLayer(datasource);
        map.layers.add(pointLayer);

        //Add the zoom control to the map.
        map.controls.add(new atlas.control.ZoomControl(), {
            position: 'top-right'
        });

        //Create a data source and add it to the map.
        datasource = new atlas.source.DataSource();
        map.sources.add(datasource);

        //Add a layer for rendering the results.
        var searchLayer = new atlas.layer.SymbolLayer(datasource, null, {
            iconOptions: {
                image: 'pin-round-darkblue',
                anchor: 'center',
                allowOverlap: true
            }
        });
        map.layers.add(searchLayer);

        //When the mouse is over the layer, change the cursor to be a pointer.
        map.events.add('mouseover', pointLayer, function () {
            map.getCanvasContainer().style.cursor = 'pointer';
        });

        //When the mouse leaves the item on the layer, change the cursor back to the default which is grab.
        map.events.add('mouseout', pointLayer, function () {
            map.getCanvasContainer().style.cursor = 'grab';
        });

        //Add a click event to the search layer and show a popup when a result is clicked.
        map.events.add("click", searchLayer, function (e) {
            //Make sure the event occurred on a shape feature.
            if (e.shapes && e.shapes.length > 0) {
                showPopup(e.shapes[0]);
            }
        });

        //Add click events to the polygon and line layers.
        map.events.add('click', [pointLayer], featureClicked);

        //Add a style control to the map.
        map.controls.add(new atlas.control.StyleControl({
            //Optionally specify which map styles you want to appear in the picker. 
            //All styles available with the S0 license tier appear by default in the control. 
            //If using a S1 tier license, you can use the mapStyles option to add premium styles such as 'satellite' and 'satellite_road_labels' to the control.
            //To add all available styles, you can use the 'all' keyword.
            mapStyles: 'all'

            //Alternatively, specify an array of all the map styles you would like displayed in the style picker.
            //mapStyles: ['road', 'road_shaded_relief', 'grayscale_light', 'night', 'grayscale_dark', 'satellite', 'satellite_road_labels', 'high_contrast_dark']

            //Customize the layout of the style picker to be a list scrollable list.
            //,layout: 'list'
        }), {
                position: 'bottom-left'
            });

        //Create a popup but leave it closed so we can update it and display it later.
        popup = new atlas.Popup();
    });
}
function searchInputKeyup(e) {
    centerMapOnResults = false;
    if (searchInput.value.length >= minSearchInputLength) {
        if (e.keyCode === 13) {
            centerMapOnResults = true;
        }
        //Wait 100ms and see if the input length is unchanged before performing a search. 
        //This will reduce the number of queries being made on each character typed.
        setTimeout(function () {
            if (searchInputLength == searchInput.value.length) {
                search();
            }
        }, keyStrokeDelay);
    } else {
        resultsPanel.innerHTML = '';
    }
    searchInputLength = searchInput.value.length;
}
function search() {
    //Remove any previous results from the map.
    datasource.clear();
    popup.close();
    resultsPanel.innerHTML = '';

    //Use MapControlCredential to share authentication between a map control and the service module.
    var pipeline = atlas.service.MapsURL.newPipeline(new atlas.service.MapControlCredential(map));

    //Construct the SearchURL object
    var searchURL = new atlas.service.SearchURL(pipeline);

    var query = document.getElementById("search-input").value;
    searchURL.searchPOI(atlas.service.Aborter.timeout(10000), query, {
        lon: map.getCamera().center[0],
        lat: map.getCamera().center[1],
        maxFuzzyLevel: 4,
        view: 'Auto'
    }).then((results) => {

        //Extract GeoJSON feature collection from the response and add it to the datasource
        var data = results.geojson.getFeatures();
        datasource.add(data);

        if (centerMapOnResults) {
            map.setCamera({
                bounds: data.bbox
            });
        }
        console.log(data);
        //Create the HTML for the results list.
        var html = [];
        for (var i = 0; i < data.features.length; i++) {
            var r = data.features[i];
            html.push('<li onclick="itemClicked(\'', r.id, '\')" onmouseover="itemHovered(\'', r.id, '\')">')
            html.push('<div class="title">');
            if (r.properties.poi && r.properties.poi.name) {
                html.push(r.properties.poi.name);
            } else {
                html.push(r.properties.address.freeformAddress);
            }
            html.push('</div><div class="info">', r.properties.type, ': ', r.properties.address.freeformAddress, '</div>');
            if (r.properties.poi) {
                if (r.properties.phone) {
                    html.push('<div class="info">phone: ', r.properties.poi.phone, '</div>');
                }
                if (r.properties.poi.url) {
                    html.push('<div class="info"><a href="http://', r.properties.poi.url, '">http://', r.properties.poi.url, '</a></div>');
                }
            }
            html.push('</li>');
            resultsPanel.innerHTML = html.join('');
        }

    });
}

function itemHovered(id) {
    //Show a popup when hovering an item in the result list.
    var shape = datasource.getShapeById(id);
    showPopup(shape);
}

function itemClicked(id) {
    //Center the map over the clicked item from the result list.
    var shape = datasource.getShapeById(id);
    map.setCamera({
        center: shape.getCoordinates(),
        zoom: 17
    });
}

function showPopup(shape) {
    var properties = shape.getProperties();
    //Create the HTML content of the POI to show in the popup.
    var html = ['<div class="poi-box">'];
    //Add a title section for the popup.
    html.push('<div class="poi-title-box"><b>');

    if (properties.poi && properties.poi.name) {
        html.push(properties.poi.name);
    } else {
        html.push(properties.address.freeformAddress);
    }
    html.push('</b></div>');
    //Create a container for the body of the content of the popup.
    html.push('<div class="poi-content-box">');
    html.push('<div class="info location">', properties.address.freeformAddress, '</div>');
    if (properties.poi) {
        if (properties.poi.phone) {
            html.push('<div class="info phone">', properties.phone, '</div>');
        }
        if (properties.poi.url) {
            html.push('<div><a class="info website" href="http://', properties.poi.url, '">http://', properties.poi.url, '</a></div>');
        }
    }
    html.push('</div></div>');
    popup.setOptions({
        position: shape.getCoordinates(),
        content: html.join('')
    });
    popup.open(map);
}

function featureClicked(e) {
    //Make sure the event occurred on a shape feature.
    if (e.shapes && e.shapes.length > 0) {
        //By default, show the popup where the mouse event occurred.
        var pos = e.position;
        var offset = [0, 0];
        var properties;

        if (e.shapes[0] instanceof atlas.Shape) {
            properties = e.shapes[0].getProperties();

            //If the shape is a point feature, show the popup at the points coordinate.
            if (e.shapes[0].getType() === 'Point') {
                pos = e.shapes[0].getCoordinates();
                offset = [0, -18];
            }
        } else {
            properties = e.shapes[0].properties;

            //If the shape is a point feature, show the popup at the points coordinate.
            if (e.shapes[0].type === 'Point') {
                pos = e.shapes[0].geometry.coordinates;
                offset = [0, -18];
            }
        }

        var popupContent = document.createElement('div');
        console.log(properties);
        loadData(popupContent, properties.OBJECTID);

        //Update the content and position of the popup.
        popup.setOptions({
            //Create a table from the properties in the feature.
            //content: atlas.PopupTemplate.applyTemplate(properties),
            content: popupContent,
            position: pos,
            pixelOffset: offset
        });

        //Open the popup.
        popup.open(map);
    }
}