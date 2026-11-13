/**
* Sidebar V2 - Mobile Tab Navigation
*/


(function () {
    'use strict';

    // Configuration
    const BREAKPOINT = 992; // px - matches CSS media query

    let isMobile = false;

    function checkIsMobile() {
        return window.innerWidth < BREAKPOINT;
    }


    function init() {
        isMobile = checkIsMobile();

        if (!isMobile) {
            return; // Don't initialize on desktop
        }
    }

})();
