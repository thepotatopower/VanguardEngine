-- 炎華のドラグリッターガール ラディリナ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("ACTCondition")
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetProperty(p.Glitter)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function ACTCondition()
	return not obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt05_t01"), q.UnitType, u.Token})
end

function ACTCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function ACT()
	obj.SuperiorCall({}, {}, 1, 1, "dbt05_t01", {}, {})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.GetNumberOf("Filter", {l.Units}) > 0
end

function Filter(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.NameContains(id, obj.LoadName("Rorowa")) and obj.HasProperty(id, p.IsGlitter)
end

function OnAttackCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerUnits, q.UnitType, u.Token, q.Other, o.SameColumn})
end

function OnAttack()
	obj.Select({q.Location, l.PlayerUnits, q.UnitType, u.Token, q.Other, o.SameColumn, q.Count, 1})
	local power = obj.GetPower({q.Location, l.Selected})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end