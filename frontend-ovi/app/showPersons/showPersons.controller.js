'use strict';

function showPersonsController($routeParams, $location, getRequestPerson) {
    var self = this;

    self.personUri = decodeURIComponent($routeParams.personUri);

    self.$onInit = () => {
        self.persons = [];
        self.personsNumber = 0;
        self.fetchData(self.personUri);
    }

    self.fetchData = (uri) => {
        getRequestPerson.get(uri).then(
            (response) => {
                self.persons = response.data;
                self.personsNumber = self.persons.length;
            },
            (error) => {
                self.persons = error.statusText;
            }
        );
    }
    
    // self.showMovieInfo = (subject) => {
    //     if (subject && subject.includes('movie')) {
    //         $location.path('/movieInfo/' + encodeURIComponent(subject));
    //     }
    // }

    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
