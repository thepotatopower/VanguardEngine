-- Darkness Maiden, Macha

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.NameContains, "Blaster", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Looking, q.Grade, 1, q.Grade, 0, q.Other, o.Unit, q.Count, 1, q.Min, 0
	elseif n == 5 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, true, p.SB, 1, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.AddTempPower(2, 5000)
		if obj.CanSB(3) and obj.CanRetire(5) and obj.YesNo("Soul Blast 1 and retire another rear-guard?") then
			obj.SoulBlast(3)
			obj.ChooseRetire(5)
			obj.LookAtTopOfDeck(5)
			obj.SuperiorCall(4)
			obj.Shuffle()
		end
	end
	return 0
end