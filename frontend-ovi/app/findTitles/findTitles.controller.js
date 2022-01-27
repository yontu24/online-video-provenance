'use strict';

function findTitlesController($interval, getRequestMovieInfo, processMovieInfo) {
    var self = this;
    self.$onInit = () => {
        self.isMovieFound = false;
        self.titlesArray = [];
    }

    self.fetchData = (title) => {
        if (title) {
            getRequestMovieInfo.get(title).then(
                (response) => {
                    self.titlesArray = processMovieInfo.getMovieInfo(JSON.stringify(response.data));
                    self.titlesNumber = Object.keys(self.titlesArray).length;
                },
                (error) => {
                    self.titlesArray = error.statusText;
                }
            );
        }
    }

    self.findMovieByTitle = (title) => {
        console.log('Am selectat titlul ' + title);

        $interval(() => {
            if (!self.isMovieFound) {
                self.isMovieFound = true;

                self.fetchData(title);
            }
        }, 1000);
    }
}
