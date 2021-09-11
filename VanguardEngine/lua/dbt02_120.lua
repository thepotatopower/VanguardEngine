-- Overcoming the Unnatural Death

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Other, o.Order, q.Count, 2, q.Min, 0
	elseif n == 2 then
		return q.Location, l.PlayedOrder, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
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
		obj.Bind(2)
		obj.ChooseAddToHand(1)
	end
	return 0
end