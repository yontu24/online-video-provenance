app.factory('getRequestTitle', ($http) => {
    function get(title) {
        return $http.get('http://localhost:8080/ovi/scripts/response_1643045839773.json');}
    return {
        get: get
    }
}).factory('getRequestMovieInfo', ($http) => {
    function get(title) {
        return $http.get('http://localhost:8080/ovi/scripts/response_1643045839773.json');
    }
    return {
        get: get
    }
}).factory('manipulateData', () => {
    function getMovieInfo(data) {   // json
        data = JSON.parse(data);
        var title = Object.keys(data)[0];
        var movieProps = data[title];
        var newProps = {};

        for (var property in movieProps) {
            if (movieProps.hasOwnProperty(property)) {
                newProps[property.split('#').pop()] = movieProps[property];
            }
        }

        console.log(newProps);
        return newProps;
    }
    return {
        getMovieInfo: getMovieInfo
    }
});