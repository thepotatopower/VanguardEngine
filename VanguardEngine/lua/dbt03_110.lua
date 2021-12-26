-- クリード・アサルト

function RegisterAbilities()
	-- attack restriction
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("AttackRestriction")
	-- during attack
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("DuringAttack")
end

function AttackRestriction()
	if not obj.PlayerHasState(ps.RearguardStoodByEffectThisTurn) then
		obj.AddCardState({q.Location, l.PlayerRC, q.Other, o.This}, cs.CannotAttack, p.Continuous)
	end
end

function DuringAttack()
	if obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked}) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 10000, p.Continuous)
	end
end