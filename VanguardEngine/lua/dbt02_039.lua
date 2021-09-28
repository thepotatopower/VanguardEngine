-- Cardinal Fang, Marisma

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.Other, o.World, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Looking
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
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

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(3)
		obj.ChooseAddToHand(1)
		obj.RearrangeOnBottom(2)
	end
	return 0
end