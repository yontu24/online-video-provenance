'use strict';

angular.module('movieInfo')
    .factory('getRequestMovieInfo', ($http) => {
        function get(uri) {
            return $http.get('http://localhost:5000/movies/data/' + encodeURI(uri));
        }
        return {
            get: get
        }
    }).factory('processMovieInfo', () => {
        function getMovieInfo(data) {   // json
            data = JSON.parse(data);

            var titleUri = 'http://www.wade-ovi.org/resources#title';
            var titlePropertyLiteral = 'title';

            var movieUri = Object.keys(data)[0];
            var movieTriples = data[movieUri];
            var newProps = {};

            for (var property in movieTriples) {
                if (movieTriples.hasOwnProperty(property)) {
                    newProps[property.split('#').pop()] = movieTriples[property];
                }
            }

            return {
                triples: movieTriples,
                title: newProps[titlePropertyLiteral] == undefined ? 
                    "Can't track information about this movie because title property is not found." : newProps[titlePropertyLiteral][titleUri],
                movieUri: movieUri
            }
        }
        return {
            getMovieInfo: getMovieInfo
        }
    });
