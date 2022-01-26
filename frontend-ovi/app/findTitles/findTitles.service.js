app.factory('getRequestMovieInfo', ($http) => {
    function get(title) {
        return $http.get('http://localhost:8080/ovi/scripts/response_1643045839773.json');
        // return $http.get('http://localhost:58397/getTitleInfo/' + title);
    }
    return {
        get: get
    }
}).factory('processMovieInfo', () => {
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
