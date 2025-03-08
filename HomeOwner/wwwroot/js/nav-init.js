$(function () {
    $('[data-classy-nav]').each(function () {
        var $nav = $(this);
        // Build options from data attributes
        var options = {
            theme: $nav.data('theme') || 'light',
            breakpoint: $nav.data('breakpoint') || 991,
            openCloseSpeed: $nav.data('open-close-speed') || 300,
            alwaysHidden: $nav.data('always-hidden') === true,
            openMobileMenu: $nav.data('open-mobile-menu') || 'left',
            dropdownRtl: $nav.data('dropdown-rtl') === true,
            stickyNav: $nav.data('sticky-nav') === true,
            stickyFooterNav: $nav.data('sticky-footer-nav') === true
        };

        $nav.classyNav(options);
    });
});
