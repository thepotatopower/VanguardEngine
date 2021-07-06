-- Cursed Souls Squirming in Agony

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.Soul, q.Count, 2
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 1, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Looking, q.Count, 2, q.Min, 0
	elseif n == 4 then
		return q.Location, l.Looking
	elseif n == 5 then
		return q.Location, l.LastCalled
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.AlchemagicSB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanSB(1) then 
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(4)
		if obj.IsAlchemagic() then
			obj.SuperiorCall(3)
		else
			obj.SuperiorCall(2)
		end
		obj.AddToDrop(4)
		obj.AddTempPower(5, 5000)
	end
	return 0
end