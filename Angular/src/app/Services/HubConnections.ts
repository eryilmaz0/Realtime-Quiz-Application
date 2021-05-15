import { HubConnection } from "@aspnet/signalr";
import { ConnectionHubService } from "./connection-hub.service";

export class HubConnections{

    public static ConnectionHubConnection:HubConnection;
    public static MatchHubConnection:HubConnection;

}