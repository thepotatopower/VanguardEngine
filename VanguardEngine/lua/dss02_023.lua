-- 柩機の徒 リージョ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRetire)
	ability1.SetLocation(l.RC)
	ability1.SetMovedFrom(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPlayerTurn() and obj.Exists({q.Location, l.Applicable, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Other, o.Player})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end