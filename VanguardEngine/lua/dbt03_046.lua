-- ドリリングル・エンジェル

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.Drop, q.Other, o.Unit, q.Count, 1}) end
	local added = obj.ChooseAddToSoul({q.Location, l.Drop, q.Other, o.Unit, q.Count, 1})
	obj.Store(added)
end

function Activation()
	obj.LookAtTopOfDeck(3)
	obj.SuperiorCall({q.Location, l.Looking, q.Name, obj.GetName({q.Location, l.Stored}), q.Count, 1, q.Min, 0})
	obj.Shuffle()
end