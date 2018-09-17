Option Explicit On
Option Strict On
Option Infer On

Public Class ConfirmGoodsReceiveDetails


    Private LOTNo As String
    Public Property setLOTNo() As String
        Get
            Return LOTNo
        End Get
        Set(ByVal value As String)
            LOTNo = value
        End Set
    End Property
    Private WHSite As String
    Public Property setWHSite() As String
        Get
            Return WHSite
        End Get
        Set(ByVal value As String)
            WHSite = value
        End Set
    End Property
    Private CustomerLOTNo As String
    Public Property setCustomerLOTNo() As String
        Get
            Return CustomerLOTNo
        End Get
        Set(ByVal value As String)
            CustomerLOTNo = value
        End Set
    End Property
    Private ItemNo As String
    Public Property setItemNo() As String
        Get
            Return ItemNo
        End Get
        Set(ByVal value As String)
            ItemNo = value
        End Set
    End Property
    Private ProductCode As String
    Public Property setProductCode() As String
        Get
            Return ProductCode
        End Get
        Set(ByVal value As String)
            ProductCode = value
        End Set
    End Property
    Private CustomerPN As String
    Public Property setCustomerPN() As String
        Get
            Return CustomerPN
        End Get
        Set(ByVal value As String)
            CustomerPN = value
        End Set
    End Property
    Private OwnerPN As String
    Public Property setOwnerPN() As String
        Get
            Return OwnerPN
        End Get
        Set(ByVal value As String)
            OwnerPN = value
        End Set
    End Property
End Class
