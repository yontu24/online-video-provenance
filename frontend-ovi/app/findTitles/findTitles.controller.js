'use strict';

function findTitlesController($routeParams, $route, $location, $timeout, manipulateData, getRequestTitlesFromDataset, getRequestTitlesFromAnotherSource) {
    var self = this;

    self.titleParam = decodeURIComponent($routeParams.titleId);
    
    self.$onInit = () => {
        self.isMovieFound = false;
        self.titlesArray = [];
        self.message = '';
        self.foundOnDataset = true;
        self.fetchData(self.titleParam);
    }

    self.fetchData = (uri) => {
        if (uri) {
            getRequestTitlesFromDataset.get(uri).then(
                (response) => {
                    self.message = 'Movies found on dataset:'
                    self.titlesArray = manipulateData.getTitles(JSON.stringify(response.data));
                    self.titlesNumber = Object.keys(self.titlesArray).length;

                    if (!self.titlesNumber) {
                        self.foundOnDataset = false;
                        self.message = 'Movies found on dbpedia:';
                        self.fetchDataFromAnotherSource(self.titleParam);
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
                
                if (!self.titlesNumber) {
                    self.message = 'No results. Redirecting to search page.';
                    $timeout(() => {
                        $location.path('/search');
                    }, 2000);
                }
            },
            (error) => {
                self.titlesArray = error.statusText;
            }
        );
    }

    self.displayMovieInfo = (uri) => {
        uri = encodeURIComponent(uri);
        if (self.foundOnDataset) {
            $timeout(() => {
                if (!self.isMovieFound) {
                    self.isMovieFound = true;
                    $location.path('/movieInfo/' + uri);
                }
            }, 1000);
        } else {
            self.message = 'Current page is reloading. Be patient! Adding these movies on dataset...';
            $timeout(() => {
                $route.reload();
            }, 1000);
        }
    }

    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
