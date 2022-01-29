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

            console.log(data);

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
