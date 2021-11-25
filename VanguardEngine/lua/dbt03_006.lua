-- 柩機の竜 デスティアーデ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetActivation("Cont")
	-- on battle ends
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetActivation("OnBattleEnds")
end

function Cont()
	if obj.IsAbyssalDarkNight() then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	elseif obj.IsDarkNight() then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCanFullyResolve()
	return obj.IsSameZone()
end

function OnBattleEndsCost(check)
	if check then return obj.Exists({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Count, 3}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Count, 3})
end

function OnBattleEnds()
	if obj.IsSameZone() then
		obj.Stand({q.Location, l.PlayerRC, q.Other, o.This})
	end
end