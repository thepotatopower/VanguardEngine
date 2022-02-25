-- 黙示録の風

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt02_001")})
end

function Activation()
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt02_001")}) then
		obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 20000, p.UntilEndOfBattle)
	end
end