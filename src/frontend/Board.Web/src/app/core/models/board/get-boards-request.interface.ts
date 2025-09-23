export interface GetBoardsRequest {
    page?: number;
    pageSize?: number;
    titleSearchTerm?: string | null;
    ownerSearchTerm?: string | null;
  }