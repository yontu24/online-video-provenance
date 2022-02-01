'use strict';

function searchMovieFormController($location) {
    var self = this;

    self.findTitles = (title) => {
        if (title) {
            $location.path('/findTitle/' + title);
            title = '';
        }
    };
}
