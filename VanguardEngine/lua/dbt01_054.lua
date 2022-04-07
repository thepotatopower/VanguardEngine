-- 悲嘆と絶望、そして拒絶

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function GetCosts()
	return p.CB, 1
end

function Activation()
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_009")}) then
		obj.Select({q.Location, l.PlayerUnits, q.Count, 3})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	end
end