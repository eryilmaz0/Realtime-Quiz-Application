import {ResponseModel} from './ResponseModel'

export interface DataResponseModel<T> extends ResponseModel
{
    data:T;
}