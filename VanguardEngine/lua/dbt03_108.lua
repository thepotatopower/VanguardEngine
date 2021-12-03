-- 真理を照らす光

function RegisterAbilities()
	-- blitz order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt01_008")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt01_008")}, q.Count, 1)
	if obj.GetNumberOf({q.Location, l.Selected}) > 0 and obj.CanCB(2) and obj.YesNo(obj.GetDescription(2)) then
		obj.CounterBlast(2)
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 40000, p.UntilEndOfBattle)
	else
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfBattle)
	end
end