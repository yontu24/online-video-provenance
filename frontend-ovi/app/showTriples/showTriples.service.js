'use strict';

angular.module('showTriples')
    .factory('getRequestTriples', ($http) => {
        return {
            get: get
        }

        function get(propertyUri) {
            return $http.get('http://localhost:5000/resources/' + encodeURIComponent(propertyUri));
        }
    });
