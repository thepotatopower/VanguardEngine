-- ユースベルク "天壁黎騎"

function RegisterAbilities()
	-- treated as grand march lianorn
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Cont")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
	-- +5000
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetTiming(a.Cont)
	ability3.SetLocation(l.RC)
	ability3.SetActivation("Cont2")
end

function Cont()
	obj.AddCardValue({q.Other, o.This}, cs.RegardedAsWhenRodeUpon, "Grand March of Full Bloom, Lianorn", p.Continuous)
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCost(check)
	if check then return obj.CanSB(1) and obj.CanDiscard(1) end
	obj.SoulBlast(1)
	obj.Discard(1)
end

function OnAttack()
	obj.Boost({q.Location, l.PlayerVC}, {q.Location, l.BackRow})
	if obj.GetNumberOf({q.Location, l.PlayerUnits, q.Other, o.Booster}) >= 3 then
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusDrive, 1, p.UntilEndOfBattle)
		local given = obj.GiveAbility(GetCardID(), GetCardID())
		given.SetTiming(a.OnBattleEnds)
		given.SetResetTiming(a.OnBattleEnds)
		given.SetCondition("GivenCondition")
		given.SetActivation("Given")
	end
end

function GivenCondition()
	return obj.CanStand({q.Location, l.BackRowRC})
end

function Given()
	obj.ChooseStand({q.Location, l.BackRowRC, q.Count, 2})
end

function Cont2()
	if obj.IsAttackingUnit() and obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Booster}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end