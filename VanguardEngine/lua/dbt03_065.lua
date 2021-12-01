-- 砂塵の猟弾 セドリック

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsPlayerTurn() and not obj.Exists({q.Location, l.EnemyRC, q.Count, 3}) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end