export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  statusCode: number;
}


export interface PaginatedResponse<T> {
  data: T[];
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

export interface TransactionRequestParams {
  Page?: number;
  PageSize?: number;
  SortColumn?: string;
  SortDirection?: 'asc' | 'desc';
  FilterType?: string;
  FilterCategory?: string;
  StartDate?: string;
  EndDate?: string;
  MinAmount?: number | null;
  MaxAmount?: number | null;
  SearchText?: string;
}