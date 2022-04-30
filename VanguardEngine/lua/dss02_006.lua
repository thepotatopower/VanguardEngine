-- 極光戦姫 ランチャー・シャルリーン

function RegisterAbilities()
	-- guard restrict
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont1")
	-- restraint
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont2")
end

function Cont1()
	if obj.IsAttackingUnit() then
		obj.AddEnemyState(ps.CannotCallNormalUnitsToGC, p.Continuous)
		obj.AddEnemyState(ps.CannotPlayBlitzOrderFromHand, p.Continuous)
	end
end

function Cont2()
	if obj.GetNumberOf({q.Location, l.PlayerPrisoners}) <= 5 then
		obj.AddCardState({q.Other, o.This}, cs.CannotAttack, p.Continuous)
	end
end