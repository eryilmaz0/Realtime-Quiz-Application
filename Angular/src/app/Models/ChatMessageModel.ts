import { UserModel } from "./UserModel";

export interface ChatMessageModel{

    user:UserModel;
    message:string;
}