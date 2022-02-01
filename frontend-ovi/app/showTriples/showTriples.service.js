'use strict';

angular.module('showTriples')
    .factory('getRequestTriples', ($http) => {
        return {
            get: get
        }

        function get(propertyUri) {
            var url = 'http://localhost:5000/resources/' + encodeURIComponent(propertyUri);
            return $http.get(url);
        }
    }).factory('getRequestPerson', ($http) => {
        return {
            get: get
        }

        function get(personUri) {
            var url = 'http://localhost:5000/dbpedia/persons/' + encodeURIComponent(personUri);
            return $http.get(url);
        }
    });
