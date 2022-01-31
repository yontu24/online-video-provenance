'use strict';

// encode la recomandari
// de pus param la func title 25

function findTitlesController($routeParams, $location, $interval, manipulateData, getRequestTitlesFromDataset, getRequestTitlesFromAnotherSource) {
    var self = this;

    self.titleId = $routeParams.titleId;
    
    self.$onInit = () => {
        self.isMovieFound = false;
        self.titlesArray = [];
        self.fetchData(self.titleId);
    }

    self.fetchData = (uri) => {
        if (uri) {
            getRequestTitlesFromDataset.get(uri).then(
                (response) => {
                    self.titlesArray = manipulateData.getTitles(JSON.stringify(response.data));
                    self.titlesNumber = Object.keys(self.titlesArray).length;

                    if (!self.titlesNumber) {
                        self.fetchDataFromAnotherSource(title);
                    }
                },
                (error) => {
                    self.titlesArray = error.statusText;
                }
            );
        }
    }

    self.fetchDataFromAnotherSource = (uri) => {
        getRequestTitlesFromAnotherSource.get(uri).then(
            (response) => {
                self.titlesArray = manipulateData.getTitles(JSON.stringify(response.data));
                self.titlesNumber = Object.keys(self.titlesArray).length;
            },
            (error) => {
                self.titlesArray = error.statusText;
            }
        );
    }

    self.displayMovieInfo = (uri) => {
        uri = encodeURIComponent(uri);
        $interval(() => {
            if (!self.isMovieFound) {
                self.isMovieFound = true;
                $location.path('/movieInfo/' + uri);
            }
        }, 1000);
    }
}
