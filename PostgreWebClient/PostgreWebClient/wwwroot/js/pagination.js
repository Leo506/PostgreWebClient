function PaginationModel() {
    return {
        currentPage: 1,
        totalCount: 1,
        pageSize: 10
    }
}

function hasNextPage(paginationModel) {
    return (paginationModel.currentPage + 1) * paginationModel.pageSize <= paginationModel.totalCount;
}

function hasPreviousPage(paginationModel) {
    return paginationModel.currentPage > 1;
}