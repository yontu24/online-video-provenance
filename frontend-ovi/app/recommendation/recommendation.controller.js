'use strict';

function recommendationController(getRequestRecommendedTitles, concatenateUris) {
    var self = this;
    self.$onInit = () => {
        // self.recommendedTitles = this.displayTitles()();
        self.recommendedTitles = self.displayTitles;
        var params = {
            uriMovieTitle: self.movieUri,
            uriDirectors: self.displayTitles['directedBy'],
            uriActors: self.displayTitles['starring'],
            uriGenre: self.displayTitles['genre']
        };
        console.log(JSON.stringify(params, undefined, 4));
        self.fetchRecommendationData(params);
    }

    self.fetchRecommendationData = (params) => {
        var keys = Object.keys(params);

        var processedParams = {
            uriMovieTitle: encodeURIComponent(params.uriMovieTitle),
            uriDirectors: encodeURIComponent(concatenateUris.get(params[keys[1]])),
            uriActors: encodeURIComponent(concatenateUris.get(params[keys[2]])),
            uriGenre: encodeURIComponent(concatenateUris.get(params[keys[3]]))
        };
        
        console.log(JSON.stringify(processedParams, undefined, 4));

        if (processedParams) {
            getRequestRecommendedTitles.get(processedParams).then(
                (response) => {
                    self.recommendedTitles = response.data;
                },
                (error) => {
                    self.recommendedTitles = error.statusText;
                });
        }
    }
}
