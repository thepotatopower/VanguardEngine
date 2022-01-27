-- 柩機の竜 マスリア

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
end

function ACTCost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Count, 1}) end
	obj.ChooseRetire({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Count, 1})
end

function ACT()
	obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	if obj.GetActivationCount() == 3 then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusCritical, 1, p.UntilEndOfTurn)
	end
end