'use strict';

function findTitlesController($routeParams, $route, $location, $timeout, manipulateData, getRequestTitlesFromDataset, getRequestTitlesFromAnotherSource) {
    var self = this;

    self.titleParam = encodeURIComponent($routeParams.titleId);
    
    self.$onInit = () => {
        self.fetchData(decodeURIComponent(self.titleParam));
    }

    self.fetchData = (title) => {
        if (title) {
            getRequestTitlesFromDataset.get(title).then(
                (response) => {
                    self.message = 'Movies found'
                    self.titlesArray = manipulateData.getTitles(JSON.stringify(response.data));
                    self.titlesNumber = Object.keys(self.titlesArray).length;

                    if (!self.titlesNumber) {
                        self.message = 'Searching for movies on dbpedia...';
                        self.fetchDataFromAnotherSource(title);
                    }
                },
                (error) => {
                    self.titlesArray = error.statusText;
                }
            );
        }
    }

    self.fetchDataFromAnotherSource = (title) => {
        getRequestTitlesFromAnotherSource.get(title).then(
            (response) => {
                self.titlesArray = manipulateData.getTitles(JSON.stringify(response.data));
                self.titlesNumber = Object.keys(self.titlesArray).length;
                
                if (!self.titlesNumber) {
                    self.message = 'No results. Redirecting to search page.';
                    $timeout(() => {
                        $location.path('/search');
                    }, 1000);
                }
                else
                    $route.reload();
            },
            (error) => {
                self.titlesArray = error.statusText;
            }
        );
    }

    self.displayMovieInfo = (uri) => {
        uri = encodeURIComponent(uri);
        $timeout(() => {
            if (!self.isMovieFound) {
                self.isMovieFound = true;
                $location.path('/movieInfo/' + uri);
            }
        }, 1000);
    }

    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
