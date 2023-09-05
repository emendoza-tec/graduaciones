export interface BaseResponse<T> {
  message: string;
  hasError: boolean;
  result: T;
}
