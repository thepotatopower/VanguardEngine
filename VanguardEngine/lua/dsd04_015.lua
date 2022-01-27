-- 獣を呼ぶ声

function RegisterAbilities()
	-- blitz order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	if obj.Exists({q.Location, l.BackRowRC, q.Count, 3}) then
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 15000, p.UntilEndOfBattle)
	else
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfBattle)
	end
end