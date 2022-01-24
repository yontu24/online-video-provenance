app.factory('getRequestTitle', ($http) => {
    function get(title) {
        return $http.get('https://dbpedia.org/sparql?default-graph-uri=http%3A%2F%2Fdbpedia.org&query=SELECT+%3Fmovie+%3Fprop+Group_Concat%28distinct+%3Fvalue%2C+%27%2C+%27%29+as+%3Fvalue+%0D%0AWHERE+%7B+%0D%0A++%7Bselect+%3Fmovie+%7B%3Fmovie+a+dbo%3AFilm%7D++limit+10000%7D%0D%0A++%3Fmovie+a+schema%3ACreativeWork%3B%0D%0A++dbp%3Aname+%3Fname.%0D%0A++%3Fmovie+%3Fprop+%3Fvalue.%0D%0A++filter%28+%3Fprop+not+in+%28rdf%3Atype%29+%26%26+regex%28%3Fname%2C+%22' + title +'%22%2C+%22i%22%29%29%0D%0A%7D&format=json&timeout=30000&signal_void=on&signal_unconnected=on');
    }
    return {
        get: get
    }
}).factory('getRequestMovieInfo', ($http) => {
    function get(title) {
        return $http.get('http://localhost:8080/ovi/scripts/response_1643022684924.json');
        // return $http.get('http://localhost:8080/ovi/scripts/movieInfo-response.json');
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

        return newProps;
    }
    return {
        getMovieInfo: getMovieInfo
    }
});