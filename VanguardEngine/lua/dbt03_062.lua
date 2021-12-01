-- スタンボルテック・ドラゴン

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.PlayerHasState(ps.VanguardAttackHitThisTurn) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 10000, p.Continuous)
	end
end