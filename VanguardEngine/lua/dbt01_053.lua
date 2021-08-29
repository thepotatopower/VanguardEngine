-- Cursed Souls Squirming in Agony

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 2, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Looking
	elseif n == 4 then
		return q.Location, l.LastCalled
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, p.SB, 2
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
		obj.LookAtTopOfDeck(4)
		if obj.IsAlchemagic() then
			obj.SuperiorCall(2)
		else
			obj.SuperiorCall(1)
		end
		obj.AddToDrop(3)
		obj.AddTempPower(4, 5000)
	end
	return 0
end