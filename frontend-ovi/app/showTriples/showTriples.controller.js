'use strict';

function showTriplesController($routeParams, $location, getRequestTriples, getRequestPerson) {
    var self = this;

    const SUCCESS_MESSAGE = 'Person has been added to triples';
    const FAILURE_MESSAGE = 'Person not found on dbpedia';
    const dbpediaRefPredicate = 'dbpediaReference';

    self.propertyUri = decodeURIComponent($routeParams.property);

    self.$onInit = () => {
        self.triples = [];
        self.triplesNumber = 0;
        self.isNodeLiteral = false;
        self.isPerson = false;
        self.objectDbpediaRef = '';
        self.responseStatus = '';
        self.fetchData(self.propertyUri);
    }

    self.fetchData = (uri) => {
        getRequestTriples.get(uri).then(
            (response) => {
                self.triples = response.data;
                self.triplesNumber = self.triples.length;
                
                self.triples.forEach((triple) => {
                    if (triple.predicate.includes(dbpediaRefPredicate) && !self.propertyUri.includes(dbpediaRefPredicate)) {
                        self.isPerson = true;
                        self.objectDbpediaRef = triple.object;

                        console.log('self.objectDbpediaRef = ' + self.objectDbpediaRef);
                    }
                });
            },
            (error) => {
                self.triples = error.statusText;
            }
        );
    }

    self.addPersonToDataset = (objectUri) => {
        if (objectUri) {
            objectUri = encodeURIComponent(objectUri);
            getRequestPerson.get(objectUri).then(
                (response) => {
                    self.responseStatus = !angular.equals(response.data, {}) ? SUCCESS_MESSAGE : FAILURE_MESSAGE;
                },
                (error) => {
                    self.responseStatus = error.status;
                }
            );
        }
    }
    
    self.showResourceInfo = (subject) => {
        if (subject) {
            if (subject.includes('movie'))
                $location.path('/movieInfo/' + encodeURIComponent(subject));
            else if (!subject.includes('http'))
                self.isNodeLiteral = true;
            else
                $location.path('/wade-ovi.org/' + encodeURIComponent(subject));
        }
    }

    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
