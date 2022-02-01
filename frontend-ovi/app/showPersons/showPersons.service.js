'use strict';

angular.module('showPersons')
    .factory('getRequestPerson', ($http) => {
        return {
            get: get
        }

        function get(personUri) {
            return $http.get('http://localhost:5000/dbpedia/persons/' + encodeURIComponent(personUri));
        }
    });
