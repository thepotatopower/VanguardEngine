-- 悠久の時を超えて

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OrderCost")
	ability1.SetActivation("OrderActivation")
	-- on place
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PlacedOnRC)
	ability2.SetLocation(l.Order)
	ability2.SetTrigger("OnPlaceTrigger")
	ability2.SetActivation("OnPlace")
end

function OrderCost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function OrderActivation()
end

function OnPlaceTrigger()
	return obj.Exists({q.Location, l.Applicable, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Other, o.Player})
end

function OnPlace()
	obj.AddCardValue({q.Location, l.Applicable, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Other, o.Player}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end