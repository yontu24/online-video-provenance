'use strict';

function showPersonsController($routeParams, $location, $timeout, getRequestPerson) {
    var self = this;

    const SUCCESS_MESSAGE = 'Person has been added to triples.';
    const FAILURE_MESSAGE = 'Person not found on dbpedia.';

    self.personUri = decodeURIComponent($routeParams.person);

    self.$onInit = () => {
        self.fetchData(self.personUri);
    }

    self.fetchData = (uri) => {
        getRequestPerson.get(uri).then(
            (response) => {
                self.responseStatus = (response.data == {}) ? FAILURE_MESSAGE : SUCCESS_MESSAGE;

                $timeout(() => {
                    $location.path('/search');
                }, 2000);
            },
            (error) => {
                self.responseStatus = error.status;
            }
        );
    }
    
    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
