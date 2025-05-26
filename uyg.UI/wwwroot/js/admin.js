$(document).ready(function () {
    // Sidebar toggle functionality
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        $('#content').toggleClass('active');
    });

    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').alert('close');
    }, 5000);

    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Add active class to current nav item
    var currentPath = window.location.pathname;
    $('#sidebar ul li a').each(function() {
        var linkPath = $(this).attr('href');
        if (currentPath.includes(linkPath)) {
            $(this).parent().addClass('active');
        }
    });

    // Confirm delete actions
    $('.delete-confirm').on('click', function(e) {
        if (!confirm('Are you sure you want to delete this item?')) {
            e.preventDefault();
        }
    });

    // Form validation
    $('form').on('submit', function() {
        var requiredFields = $(this).find('[required]');
        var isValid = true;

        requiredFields.each(function() {
            if (!$(this).val()) {
                isValid = false;
                $(this).addClass('is-invalid');
            } else {
                $(this).removeClass('is-invalid');
            }
        });

        return isValid;
    });

    // Clear form validation on input
    $('input, textarea').on('input', function() {
        $(this).removeClass('is-invalid');
    });

    // File input preview
    $('.custom-file-input').on('change', function() {
        var fileName = $(this).val().split('\\').pop();
        $(this).next('.custom-file-label').html(fileName);
    });

    // Table row hover effect
    $('.table tbody tr').hover(
        function() { $(this).addClass('table-hover'); },
        function() { $(this).removeClass('table-hover'); }
    );

    // Responsive table handling
    function handleResponsiveTables() {
        if (window.innerWidth < 768) {
            $('.table-responsive').addClass('table-responsive-sm');
        } else {
            $('.table-responsive').removeClass('table-responsive-sm');
        }
    }

    // Call on load and resize
    handleResponsiveTables();
    $(window).resize(handleResponsiveTables);
}); 