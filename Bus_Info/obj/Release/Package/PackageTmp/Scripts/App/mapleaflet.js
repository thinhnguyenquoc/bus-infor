var map, marDep = null, marArr = null, lat = null, lon = null;

//////////////////////////////////////////////////////////////

map = L.map( 'map', {
	center: [10.755769, 106.713852],
	minZoom: 1, 
	zoom: 14
});
	
//Khởi tạo và hiển thị các lớp trên bản đồ -- Thêm các đường vẽ của map
/*L.tileLayer( 'http://{s}.mqcdn.com/tiles/1.0.0/map/{z}/{x}/{y}.png', {
	attribution: '&copy; <a href="http://osm.org/copyright" title="OpenStreetMap" target="_blank">OpenStreetMap</a> contributors | Tiles Courtesy of <a href="http://www.mapquest.com/" title="MapQuest" target="_blank">MapQuest</a> <img src="http://developer.mapquest.com/content/osm/mq_logo.png" width="16" height="16">',
	subdomains: ['otile1','otile2','otile3','otile4']
}).addTo( map );*/
	
ACCESS_TOKEN = 'pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpandmbXliNDBjZWd2M2x6bDk3c2ZtOTkifQ._QA7i5Mpkd_m30IGElHziw';
MB_ATTR = 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
    '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
    'Imagery © <a href="http://mapbox.com">Mapbox</a>';
MB_URL = 'https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=' + ACCESS_TOKEN;
OSM_URL = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
OSM_ATTRIB = '&copy; <a href="http://openstreetmap.org/copyright">OpenStreetMap</a> contributors';
L.tileLayer(MB_URL, { attribution: MB_ATTR, id: 'mapbox.streets' }).addTo(map);


$(window).on("resize", function() {
    	$("#map").height($(window).height()-101).width($(window).width()+12);
    		map.invalidateSize();
}).trigger("resize");
