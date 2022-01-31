'use strict';

angular.module('titleRecommendation')
    .factory('getRequestRecommendedTitles', ($http) => {
        function get(params) {  // json
            var url = 'http://localhost:54436/recommendation?movie=' + params.uriMovieTitle;
            
            if (params.uriDirectors != "" && params.uriDirectors != undefined)
                url += '&directors=' + params.uriDirectors;

            if (params.uriActors != "" && params.uriActors != undefined)
                url += '&actors=' + params.uriActors;
                
            // if (params.uriGenre != "" && params.uriGenre != undefined)
            //     url += '&genre=' + params.uriGenre;

            console.log(url);
            return $http.get(url);
        }
        return {
            get: get
        }
    }).factory('concatenateUris', () => {
        function get(data) {  // json
            const delimiter = '|separator|';

            // some uris have no resource such as ''
            const emptyUri = 'http://www.wade-ovi.org/resources#';
            var concatenatedUri = '';

            for (var uri in data) {
                if (uri == emptyUri)
                    continue;

                concatenatedUri += delimiter + uri;
            }

            return concatenatedUri.replace(delimiter, '');
        }
        return {
            get: get
        }
    });
