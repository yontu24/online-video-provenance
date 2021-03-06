'use strict';

function recommendationController($location, getRequestRecommendedTitles, concatenateUris) {
    var self = this;

    const recommendationObj = {
        directedBy: 'http://www.wade-ovi.org/resources#directedBy',
        starring: 'http://www.wade-ovi.org/resources#starring',
        genre: 'http://www.wade-ovi.org/resources#genre'
    };

    const emptyString = '';

    self.title = '';
    self.recommendedTitles = {};
    self.params = {
        uriMovieTitle: emptyString,
        uriDirectors: emptyString,
        uriActors: emptyString,
        uriGenre: emptyString
    };
    
    self.$onInit = () => {
        if (self.movieUri != undefined) {
            self.params = {
                uriMovieTitle: encodeURIComponent(self.movieUri),
                uriDirectors: concatenateUris.get(self.displayTitles[recommendationObj.directedBy]),
                uriActors: concatenateUris.get(self.displayTitles[recommendationObj.starring]),
                uriGenre: concatenateUris.get((self.displayTitles[recommendationObj.genre] == undefined) ? 
                        emptyString : self.displayTitles[recommendationObj.genre][recommendationObj.genre])
            };
        }

        self.fetchRecommendationData(self.params);
    }

    self.fetchRecommendationData = function(params) {
        if (params) {
            getRequestRecommendedTitles.get(params).then(
                (response) => {
                    self.recommendedTitles = response.data;

                    if ((self.recommendedTitles.actors == null || angular.equals(self.recommendedTitles.actors, {})) && 
                        (self.recommendedTitles.genres == null || angular.equals(self.recommendedTitles.genres, {})) && 
                        (self.recommendedTitles.directors == null || angular.equals(self.recommendedTitles.directors, {}))) {
                        self.recommendedTitles = undefined;
                        self.title = 'No recommendation found';
                    } else
                        self.title = 'We found some recommendations';
                },
                (error) => {
                    self.recommendedTitles = error.statusText;
                });
        }
    }

    self.showMovieInfo = (uriTitle) => {
        uriTitle = encodeURIComponent(Object.keys(uriTitle)[0]);
        $location.path('/movieInfo/' + uriTitle);
    }
}
