-- 大切な物のために ロロワ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.VC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
	-- glitter
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetProperty(p.IsGlitter)
end

function ACTCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function ACT()
	obj.SuperiorCall({}, {}, 3, 1, "dbt05_t02", {}, {})
end

function OnAttackTrigger()
	return obj.IsAttacked()
end

function OnAttackCost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerRC, q.UnitType, u.Token, q.Count, 1}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.UnitType, u.Token, q.Min, 1})
end

function OnAttack()
	local power = obj.GetNumberOf({q.Location, l.SuccessfullyRetired}) * 5000
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end